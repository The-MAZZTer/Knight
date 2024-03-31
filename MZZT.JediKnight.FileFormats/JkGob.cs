using MZZT.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MZZT.FileFormats {
	public class JkGob : File<JkGob> {
		public override async Task LoadAsync(Stream stream) {
			this.position = 0;
			this.header = await stream.ReadAsync<Header>();
			this.position += Marshal.SizeOf<Header>();

			if (!this.header.IsMagicValid) {
				throw new FormatException();
			}

			if (this.header.FirstFileSizeOffset != 0x14) {
				throw new FormatException();
			}

			if (this.header.NumFilesOffset != 0x0C) {
				throw new FormatException();
			}

			List<GobItemInformation> items = [];
			for (int i = 0; i < this.header.NumFiles; i++) {
				GobItemInformation item = await stream.ReadAsync<GobItemInformation>();
				this.position += Marshal.SizeOf<GobItemInformation>();
				items.Add(item);
			}
			this.Items = [.. items];
		}
		private long position = 0;
		private Header header;
		public GobItemInformation[] Items { get; private set; }

		public GobItemInformation FindFile(string filename) {
			filename = filename.ToLower();
			return this.Items.FirstOrDefault(x => x.Name.ToLower() == filename);
		}

		public void MoveToStartOfFile(Stream stream, GobItemInformation file) {
			if (stream.CanSeek) {
				stream.Seek(file.Offset - this.position, SeekOrigin.Current);
				this.position = file.Offset;
				return;
			}

			long offset = file.Offset - this.position;
			if (offset < 0) {
				throw new NotSupportedException();
			}

			byte[] buffer = new byte[Math.Min(offset, 1024 * 1024)];
			int readBytes;
			while (offset > 0) {
				readBytes = stream.Read(buffer, 0, (int)Math.Min(offset, buffer.Length));
				offset -= readBytes;
				if (readBytes <= 0) {
					throw new EndOfStreamException();
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct Header {
			public byte Magic1;
			public byte Magic2;
			public byte Magic3;
			public byte Version;
			public int FirstFileSizeOffset;
			public int NumFilesOffset;
			public int NumFiles;

			public bool IsMagicValid =>
				this.Magic1 == (byte)'G' && this.Magic2 == (byte)'O' && this.Magic3 == (byte)'B' && this.Version == 0x20;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
		public struct GobItemInformation {
			public int Offset;
			public int Length;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string Name;
		}
	}
}
