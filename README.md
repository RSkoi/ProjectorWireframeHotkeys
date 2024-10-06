## ProjectorWireframeHotkeys

Studio plugin for KK/KKS studio that adds a 'toggle wireframes' hotkey for xukmi projector items.

This plugin will not add wireframe materials for you. Copy the main material with Material Editor and set its shader to SuperSystems/Wireframe-Transparent.

Notice that changes will not be saved to the scene, unless explicitly set in Material Editor.

### Config

- **Toggle Wireframes:** Hotkey to toggle the visibility of wireframe materials attached to xukmi projectors. (Default: P+RightCtrl)
- **Projector Shader Name:** Name of the projector shader to find. (Default: "xukmi/Effects/Projector")
- **Wireframe Shader Name:** Name of the wireframe shader to find. (Default: "Wireframe-Transparent")
- **Wireframe Shader Color Property Name:** Name of the color property of the wireframe shader to adjust. Notice that Material Editor does not display the internal '_' prefix for shader properties. (Default: "_WireColor")
