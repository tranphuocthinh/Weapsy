using System;
using System.Collections.Generic;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Infrastructure.Commands;
using Weapsy.Infrastructure.Events;

namespace Weapsy.Domain.Themes.Handlers
{
    public class HideThemeHandler : ICommandHandler<HideTheme>
    {
        private readonly IThemeRepository _themeRepository;

        public HideThemeHandler(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public IEnumerable<IEvent> Handle(HideTheme command)
        {
            var theme = _themeRepository.GetById(command.Id);

            if (theme == null)
                throw new Exception("Theme not found.");

            theme.Hide();

            _themeRepository.Update(theme);

            return theme.Events;
        }
    }
}
