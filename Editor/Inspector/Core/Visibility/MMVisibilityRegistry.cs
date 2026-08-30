namespace MM.Inspector.Editor
{
    public static class MMVisibilityRegistry
    {
        private static MMHandlerMap<MMHideProcessor> _hides;
        private static MMHandlerMap<MMDisableProcessor> _disables;

        public static MMHideProcessor GetHideProcessor(MMAttribute attribute)
        {
            _hides ??= new MMHandlerMap<MMHideProcessor>(typeof(MMHideProcessor<>));

            return _hides.Get(attribute);
        }

        public static MMDisableProcessor GetDisableProcessor(MMAttribute attribute)
        {
            _disables ??= new MMHandlerMap<MMDisableProcessor>(typeof(MMDisableProcessor<>));

            return _disables.Get(attribute);
        }
    }
}
