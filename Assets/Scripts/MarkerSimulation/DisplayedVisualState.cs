namespace GonadFateSim.MarkerSimulation
{
    public struct DisplayedVisualState
    {
        public VisualStateDescriptor Descriptor;

        public DisplayedVisualState(VisualStateDescriptor descriptor)
        {
            Descriptor = descriptor.Clamped();
        }
    }
}
