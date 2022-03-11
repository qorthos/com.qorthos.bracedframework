# KenShape Support
Limited support for .kenshape files using Unity's Asset Import Pipeline. The .kenshape files are automatically converted into KenShapeModel, a ScriptableObject like asset that contains a mesh and a list of 'kenxels'. Kenxel is a data structure that contains the position, shape, color, depth of part of a shape.

Included is a Unlit Vertex Color material to render the mesh in the KenShapeModel.

The backface of a model can be set to a specific color using the asset import pipeline.

### Limitations
As of now:
- only the first two shapes, the square and the triangle, are supported. Other shapes will be added soon.
- the mesh created is not optimized.