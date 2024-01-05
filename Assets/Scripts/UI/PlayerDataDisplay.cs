using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Munchy.UI
{
    public class PlayerDataDisplay : MonoBehaviour
    {
        [SerializeField] private Image colorImage;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI foodText;
        [SerializeField] private TextMeshProUGUI nameText;

        private void Awake()
        {
            Communication.Listen<PlayerColorChangedMessage>(RefreshColorImage, this);
            Communication.Listen<PlayerNameChangedMessage>(RefreshNameText, this);
            Communication.Listen<MoneyChangedMessage>(RefreshMoneyText, this);
            Communication.Listen<FoodChangedMessage>(RefreshFoodText, this);
        }

        private void RefreshColorImage(PlayerColorChangedMessage message) 
            => colorImage.color = message.primaryColor;

        private void RefreshNameText(PlayerNameChangedMessage message) 
            => nameText.text = message.data;

        private void RefreshMoneyText(MoneyChangedMessage message) 
            => moneyText.text = message.data.ToString();

        private void RefreshFoodText(FoodChangedMessage message) 
            => foodText.text = $"{message.foodUsed}/{message.food}";
    }
}