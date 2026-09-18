import struct, zlib

def check_fbx(path):
    print(f"=== {path} ===")
    with open(path, 'rb') as f:
        data = f.read()
    pos = data.find(b'Vertices')
    if pos == -1:
        print("No Vertices found")
        return
    pos += len(b'Vertices')
    type_code = chr(data[pos])
    arr_len, enc, comp_len = struct.unpack('<III', data[pos+1:pos+13])
    raw = data[pos+13:pos+13+comp_len] if enc == 1 else data[pos+13:pos+13+arr_len*8]
    if enc == 1:
        raw = zlib.decompress(raw)
    coords = struct.unpack(f'<{arr_len}d', raw)
    xs, ys, zs = coords[0::3], coords[1::3], coords[2::3]
    print(f"  X: range = {max(xs)-min(xs):.2f} (min={min(xs):.2f}, max={max(xs):.2f})")
    print(f"  Y: range = {max(ys)-min(ys):.2f} (min={min(ys):.2f}, max={max(ys):.2f})")
    print(f"  Z: range = {max(zs)-min(zs):.2f} (min={min(zs):.2f}, max={max(zs):.2f})")

check_fbx(r'Assets\Karakter-ler\fbx\fbx\texture\garip görünüşlü sylva.fbx')
check_fbx(r'Assets\Karakter-ler\moss\moss\moss.fbx')
