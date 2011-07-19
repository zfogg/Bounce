namespace Bounce
{
    public class IndexKey
    {
        private int key { get; set; }

        public IndexKey(int key)
        {
            this.key = key;
        }

        public static explicit operator IndexKey(int interger)
        { return new IndexKey(interger); }

        public static bool operator ==(IndexKey key1, IndexKey key2)
        { return key1.key == key2.key; }

        public static bool operator ==(IndexKey key, int ID)
        { return key.key == ID; }

        public static bool operator !=(IndexKey key, int ID)
        { return key.key != ID; }

        public static bool operator !=(IndexKey key1, IndexKey key2)
        { return key1.key != key2.key; }

        public override bool Equals(object obj)
        { return (IndexKey)obj == this; }

        public override int GetHashCode()
        { return key ^ 7; }
    }
}