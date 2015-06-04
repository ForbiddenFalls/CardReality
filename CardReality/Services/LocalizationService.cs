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
                    },
                    {
                        Message.ContactUs, "Contact us"
                    },
                    {
                        Message.Information, "Information"
                    },
                    {
                        Message.GameInfo, "Card reality is a turn-based card game for passionate players."
                    },
                    {
                        Message.Address, "Address"
                    },
                    {
                        Message.Email, "Email"
                    },
                    {
                        Message.Phone, "Phone"
                    },
                    {
                        Message.Name, "Name"
                    },
                    {
                        Message.Subject, "Subject"
                    },
                    {
                        Message.Message, "Message"
                    },
                    {
                        Message.Send, "Send"
                    },
                    {
                        Message.Ranking, "Ranking"
                    },
                    {
                        Message.About, "About"
                    },
                    {
                        Message.Contact, "Contact"
                    },
                    {
                        Message.ToBattle, "Right now!"
                    },
                    {
                        Message.ExitBattle, "Exit the battle!"
                    },
                    {
                        Message.Hello, "Hello"
                    },
                    {
                        Message.Profile, "Profile"
                    },
                    {
                        Message.Market, "Market"
                    },
                    {
                        Message.LogOff, "Log Off"
                    },
                    {
                        Message.Letters, "Letters"
                    },
                    {
                        Message.Register, "Register"
                    },
                    {
                        Message.Login, "Log in"
                    },
                    {
                        Message.OpponentFound, "Opponent found"
                    },
                    {
                        Message.GameType, "Turn-based card game"
                    },
                    {
                        Message.GameTechs, "We have developed an awesome turn-based card game with ASP.NET and SignalR serving the backend and the gameplay."
                    },
                    {
                        Message.DesignResponsive, "Responsive done"
                    },
                    {
                        Message.DesignInfo, "Although it is a more of a Back-End project we did some nice stuffs in the Front-End. Our site is fully responsive - no matter if you are viewing it on you phone, laptop, pc or even your tv at home."
                    },
                    {
                        Message.CleanCode, "Clean Code"
                    },
                    {
                        Message.CleanCodeInfo, "We have followed some of the best practices used nowadays. DRY insted of WET for example (Don't Repeat Yourself instead of We Enjoy Typing)"
                    },
                    {
                        Message.HighQuality, "High Quality Product"
                    },
                    {
                        Message.HighQualityInfo, "In the end of all this we came up with an awesome project build from the ForbiddenCity Team"
                    },
                    {
                        Message.Language, "Language"
                    },
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
                    },
                    {
                        Message.ContactUs, "Свържете се с нас"
                    },
                    {
                        Message.Information, "Информация"
                    },
                    {
                        Message.GameInfo, "Card reality е походова игра с карти със стратегически елемент"
                    },
                    {
                        Message.Address, "Адрес"
                    },
                    {
                        Message.Email, "Електронна поща"
                    },
                    {
                        Message.Phone, "Телефон"
                    },
                    {
                        Message.Name, "Име"
                    },
                    {
                        Message.Subject, "Относно"
                    },
                    {
                        Message.Message, "Съобщение"
                    },
                    {
                        Message.Send, "Изпрати"
                    },
                    {
                        Message.Ranking, "Класация"
                    },
                    {
                        Message.About, "За нас"
                    },
                    {
                        Message.Contact, "Контакти"
                    },
                    {
                        Message.ToBattle, "В битка!"
                    },
                    {
                        Message.ExitBattle, "Излез от битка!"
                    },
                    {
                        Message.Hello, "Здравей"
                    },
                    {
                        Message.Profile, "Профил"
                    },
                    {
                        Message.Market, "Търговия"
                    },
                    {
                        Message.LogOff, "Излез"
                    },
                    {
                        Message.Letters, "Букви"
                    },
                    {
                        Message.Register, "Регистрирай се"
                    },
                    {
                        Message.Login, "Влез"
                    },
                    {
                        Message.OpponentFound, "Намерен е противник"
                    },
                    {
                        Message.GameType, "Походова игра с карти"
                    },
                    {
                        Message.GameTechs, "За вас сме разработили страхотна игра с ASP.NET MVC и SignalR, която може да се играе в реално време във вашия браузър."
                    },
                    {
                        Message.DesignResponsive, "Responsive дизайн"
                    },
                    {
                        Message.DesignInfo, "Въпреки, че се стремим към back-end, нашата игра се радва и на чудесна визия. Дизайнът е напълно responsive, съответно може да я играте на вашия телефон, таблет, компютър или дори на телевизора вкъщи."
                    },
                    {
                        Message.CleanCode, "Изчистен код"
                    },
                    {
                        Message.CleanCodeInfo, "Придържали сме се към добрите практики в днешно време за чист код"
                    },
                    {
                        Message.HighQuality, "Продукт от най-високо качество"
                    },
                    {
                        Message.HighQualityInfo, "В края на деня пред вас е продукт от най-високо качество разработен от ForbiddenCity Team"
                    },
                    {
                        Message.Language, "Език"
                    },
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