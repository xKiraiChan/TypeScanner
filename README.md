# TypeScanner
Dynamic type scanner for BepInEx with caching

# Installation
This goes into your `BepInEx/plugins` folder

# Usage
- Create a `ClassDef`
  - It is recommended that you use a deserializer for this
    - An example can be seen [here](https://github.com/xKiraiChan/TypeScanner/blob/master/Example.yaml)
- Setup the `ClassDef` with `Definition.Setup`
  - Your signature must have an ID for this
- You can now access the `Resolved` property
  - If it is null, your signature yielded no results
  - The result *should* be valid (aggressive caching<sup>todo</sup> does not guarantee this)
- You can access all setup types by ID in `Definition.Table`

# Developers
Please provide feedback with the [issues](https://github.com/xKiraiChan/TypeScanner/issues) and in the [discussions](https://github.com/xKiraiChan/TypeScanner/discussions)

# Todo
- [ ] Dynamic assembly generation
- [ ] Caching modes
- [ ] `ClassDef` builder
- [ ] Built in typedef scanner
- [ ] Built in deserializers
