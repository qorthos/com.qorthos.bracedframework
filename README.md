How to add to your project: 
PackageManager -> + -> Add Package From Git URL: https://github.com/qorthos/com.qorthos.bracedframework


# KenShape Support
Support for .kenshape files using Unity's Asset Import Pipeline. The .kenshape files are automatically converted into a KenShapeModel, a ScriptableObject like asset that contains a mesh and a list of 'kenxels'. KenxelData is just a class that contains the position, shape, color, depth of part of a shape. A kenxel is a stand-alone object representing one part of a KenShapeModel that has been exploded.

Included is an unlit shader to render the mesh in the KenShapeModel and a shader to render individual kenxels.

### Limitations
- the mesh created is not optimized. Very much not optimized.