namespace Digger
{
	public class CreatureCommand
	{
		public int DeltaX;
		public int DeltaY;
		public ICreature TransformTo;
        private int v1;
        private int v2;

        public CreatureCommand()
        {
        }

        public CreatureCommand(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}