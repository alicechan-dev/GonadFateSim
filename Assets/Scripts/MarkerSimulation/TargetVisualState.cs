namespace GonadFateSim.MarkerSimulation
{
    public struct TargetVisualState
    {
        public VisualStateDescriptor Descriptor;

        public TargetVisualState(VisualStateDescriptor descriptor)
        {
            Descriptor = descriptor.Clamped();
        }
    }
}
