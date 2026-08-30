namespace MM.Inspector.Editor
{
    public static class MMVisibility
    {
        public static bool IsVisible(MMProperty property)
        {
            MMMemberSchema schema = property.Schema;
            if (schema == null)
            {
                return true;
            }

            for (int i = 0; i < schema.Hides.Length; i++)
            {
                (MMAttribute attribute, MMHideProcessor processor) = schema.Hides[i];

                if (processor.IsHidden(property, attribute))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEnabled(MMProperty property)
        {
            MMMemberSchema schema = property.Schema;
            if (schema == null)
            {
                return true;
            }

            for (int i = 0; i < schema.Disables.Length; i++)
            {
                (MMAttribute attribute, MMDisableProcessor processor) = schema.Disables[i];

                if (processor.IsDisabled(property, attribute))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
