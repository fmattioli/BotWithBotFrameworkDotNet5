using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web.Bot.CardsManager.Classes;
using Web.Bot.Models;

namespace Web.Bot.Dialogs
{
    public class AtendimentoDialog : CancelAndHelpDialog
    {
        private CardManager cardsManager { get; set; } = new CardManager();
        public AtendimentoDialog()
            : base(nameof(AtendimentoDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                MensagemExibirServicosDisponiveis,
                MensagemConsultarServicoDesejado,
                FinalStepAsync,
            }));

        }

        private async Task<DialogTurnResult> MensagemExibirServicosDisponiveis(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var detalhes = (AtendimentoDetalhes)stepContext.Options;

            var cardServicoConsultaProtocolo = cardsManager.CriarAdaptiveCard("cardServicoConsultaProtocolo");

            var cardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardServicoConsultaProtocolo),
            };

            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    AttachmentLayout = AttachmentLayoutTypes.Carousel,
                    Attachments = new List<Attachment>() {
                    cardAttachment,
                    cardAttachment
                },
                    Type = ActivityTypes.Message,
                    Text = $"Muito prazer {detalhes.NomeUsuario}! Escolha o serviço da sua necessidade para continuarmos!"
                },
            };

            return await stepContext.PromptAsync(nameof(TextPrompt), opts, cancellationToken);

        }

        private async Task<DialogTurnResult> MensagemConsultarServicoDesejado(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var cardJson = cardsManager.CriarAdaptiveCard("welcomeCard");

            var cardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson),
            };

            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Attachments = new List<Attachment>() {
                    cardAttachment
                },
                    Type = ActivityTypes.Message
                }
            };

            return await stepContext.PromptAsync(nameof(TextPrompt), opts, cancellationToken);

        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var bookingDetails = (BookingDetails)stepContext.Options;

                return await stepContext.EndDialogAsync(bookingDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

    }
}
