using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Bot.CardsManager.Interfaces
{
    public interface ICardManager
    {
        string CriarAdaptiveCard(string adaptiveName);
    }
}
