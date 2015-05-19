using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardReality.Enums;

namespace CardReality.Services
{
    public static class LocalizationService
    {
        private static Dictionary<Language, Dictionary<Message, string>> translations = new Dictionary
            <Language, Dictionary<Message, string>>()
        {
            {
                Language.En, new Dictionary<Message, string>()
                {
                    {
                        Message.CardNotOwner, "You do not own this card"
                    },
                    {
                        Message.NotEnoughMoney, "You do not have enough money"
                    },
                    {
                        Message.CardNotSold, "Card has not been sold"
                    },
                    {
                        Message.CardInvalidOffer, "Invalid offer"
                    },
                    {
                        Message.CardSameOwner, "Cannot buy your own offer"
                    },
                    {
                        Message.OfferNotBought, "Offer has not been bought"
                    }
                }
            },
            {
                Language.Bg, new Dictionary<Message, string>()
                {
                    {
                        Message.CardNotOwner, "Тази карта не е ваша"
                    },
                    {
                        Message.NotEnoughMoney, "Нямате достатъчно пари"
                    },
                    {
                        Message.CardNotSold, "Картата не беше продадена"
                    },
                    {
                        Message.CardInvalidOffer, "Невалидна оферта"
                    },
                    {
                        Message.CardSameOwner, "Не може да купите собствената си оферта"
                    },
                    {
                        Message.OfferNotBought, "Не успяхте да закупите офертата"
                    }
                }
            }
        };

        public static Language CurrentLanguage = Language.En;

        public static string Translate(Message msg)
        {
            return translations[CurrentLanguage][msg];
        }
    }
}