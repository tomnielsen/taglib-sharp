namespace TagLib.Mpeg4
{
   public class IsoChunkLargeOffsetBox : FullBox
   {
      //////////////////////////////////////////////////////////////////////////
      // private properties
      //////////////////////////////////////////////////////////////////////////
      private ulong [] offsets;
      
      
      //////////////////////////////////////////////////////////////////////////
      // public methods
      //////////////////////////////////////////////////////////////////////////
      public IsoChunkLargeOffsetBox (BoxHeader header, Box parent) : base (header, parent)
      {
         File.Seek (base.DataPosition);
         offsets = new ulong [(int) File.ReadBlock (4).ToUInt ()];
         
         for (int i = 0; i < offsets.Length; i ++)
	         offsets [i] = (ulong) File.ReadBlock (4).ToLong ();
      }
      
      private ByteVector UpdateOffsetInternal (long size_difference)
      {
      	ByteVector output = ByteVector.FromUInt ((uint) offsets.Length);
         for (int i = 0; i < offsets.Length; i ++)
         {
         	offsets [i] = (ulong) ((long)offsets [i] + size_difference);
	         output += ByteVector.FromLong ((long) offsets [i]);
         }
         
         return output;
      }
      
      public ByteVector Render (long size_difference)
      {
         
         return Render (UpdateOffsetInternal (size_difference));
      }
      
      public override ByteVector Render ()
      {
         return Render (0);
      }
      
      public void UpdateOffset (long size_difference)
      {
      	ByteVector new_data = UpdateOffsetInternal (size_difference);
         File.Insert (new_data, DataPosition, new_data.Count);
      }
      
      //////////////////////////////////////////////////////////////////////////
      // public properties
      //////////////////////////////////////////////////////////////////////////
      public          ulong [] Offsets {get {return offsets;}}
      public override ByteVector Data         {get {return null;} set {}}
   }
}