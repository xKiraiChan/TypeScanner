# TypeScanner
Dynamic type scanner for BepInEx with caching

# Installation
This goes into your `BepInEx/plugins` folder

# Usage
- Create a `ClassDef`
  - It is recommended that you use a deserializer for this but the builder pattern is supported
  - See the [builder pattern](#builder-pattern-example) and [YAML representation](#yaml-representation)
- Setup the `ClassDef` with `Definition.Setup`
  - Your signature must have an ID for this
- You can now access the `Resolved` property
  - If it is null, your signature yielded no results
  - The result *should* match the signature unless you are using aggressive caching
- You can access all setup types by ID in `Definition.Table`

# Caching modes
In the configuration file, you can choose a caching mode:
- Disabled: Never check the cache, scan every time
  - Slow: ~200 ms for the example on my machine
- Standard: Check that the type still matches the signature
  - Fast: ~2 ms
- Aggressive: Just check that the cache isn't null
  - Blazingly fast: <0 ms

# Developers
Please provide feedback with the [issues](https://github.com/xKiraiChan/TypeScanner/issues) and in the [discussions](https://github.com/xKiraiChan/TypeScanner/discussions)

# Todo
- [ ] Dynamic assembly generation
- [x] Caching modes
- [x] Builder pattern
- [ ] Built in typedef scanner
- [ ] Built in deserializers

# Builder pattern example
```cs
TypeScanner.Types.ClassDef.Create("VRC.Player")
  .FromAssembly(AppDomain.CurrentDomain
    .GetAssemblies()
    .First(x => x.GetName().Name == "Assembly-CSharp")
  )
  .DerivesFrom<MonoBehaviour>()
  .ConstructorCount(2)
  .WithMethods(
    TypeScanner.Types.MethodDef.Create()
      .WithName("OnNetworkReady")
  )
  .WithProperties(
    TypeScanner.Types.PropertyDef.Create()
      .WithType<VRC.Core.APIUser>(),
    TypeScanner.Types.PropertyDef.Create()
      .WithType<VRC.SDKBase.VRCPlayerApi>()
    )
    .Setup()
// you can now access the `Resolved` property
```

# YAML representation:
```yaml
ID: Player
Assembly: Assembly-CSharp
Derives: UnityEngine.MonoBehaviour,UnityEngine.CoreModule
Counts: 
  Constructor: 2
Methods: 
  - Name: OnNetworkReady
Properties:
  - Type: VRC.Core.APIUser,VRCCore-Standalone
  - Type: VRC.SDKBase.VRCPlayerApi,VRCSDKBase
```
