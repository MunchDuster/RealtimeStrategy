using Photon.Pun;
using System;

namespace Munchy.Networking
{
    /// <summary>
    /// Handles retreiving and updating data on photon streams
    /// AKA syncing the given variable across all clients and server
    /// </summary>
    public class StreamHandler<T>
    {
        private Action _onSet;
        private Func<PhotonStream, T, T> _serialize;

        /// <summary>
        /// Whether the value has been changed is pending to write to the photon stream ASAP
        /// </summary>
        private bool _pendingUpdate;

        /// <summary>
        /// The value to be synced
        /// </summary>
        private T _value;

        public StreamHandler(Action onSet, Func<PhotonStream, T, T> serialize, ref T value)
        {
            _onSet = onSet;
            _value = value;
            _serialize = serialize;
        }

        public void HandleStream(PhotonStream stream)
        {
            if (stream.IsWriting)
            {
                if (_pendingUpdate)
                {
                    _serialize(stream, _value);
                    _pendingUpdate = false;
                }
            }
            //Reading from stream
            else if (stream.Count > 0)
            {
                _value = _serialize(stream, _value);
                _onSet();
            }
        }

        public void Update() => _pendingUpdate = true;
    }


    // Common uses made easy
    public class Vector2ShortHandler : StreamHandler<Vector2Short>
    {
        public Vector2ShortHandler(Action onSet, ref Vector2Short value) :
            base(onSet, (stream, vec) => { vec.Serialize(stream); return vec; }, ref value)
        { }
    }

    public class ShortHandler : StreamHandler<short>
    {
        public ShortHandler(Action onSet, ref short value) :
            base(onSet, (stream, vec) => { stream.Serialize(ref vec); return vec; }, ref value)
        { }
    }
}