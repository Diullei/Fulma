namespace Fulma

open Fulma
open Fable.Import.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

[<RequireQualifiedAccess>]
module Button =

    module Classes =
        let [<Literal>] Container = "button"
        module List =
            let [<Literal>] Container = "buttons"
            module Size =
                let [<Literal>] AreSmall = "are-small"
                let [<Literal>] AreMedium = "are-medium"
                let [<Literal>] AreLarge = "are-large"

    type Option =
        // Colors
        | Color of IColor
        | Size of ISize
        /// Add `is-fullwidth` class
        | [<CompiledName("is-fullwidth")>] IsFullWidth
        /// Add `is-link` class
        | [<CompiledName("is-link")>] IsLink
        /// Add `is-outlined` class
        | [<CompiledName("is-outlined")>] IsOutlined
        /// Add `is-inverted` class
        | [<CompiledName("is-inverted")>] IsInverted
        /// Add `is-text` class
        | [<CompiledName("is-text")>] IsText
        /// Add `is-rounded` class
        | [<CompiledName("is-rounded")>] IsRounded
        /// Add `is-expanded` class
        | [<CompiledName("is-expanded")>] IsExpanded
        /// Add `is-hovered` class if true
        | [<CompiledName("is-hovered")>] IsHovered of bool
        /// Add `is-focused` class if true
        | [<CompiledName("is-focused")>] IsFocused of bool
        /// Add `is-active` class if true
        | [<CompiledName("is-active")>] IsActive of bool
        /// Add `is-loading` class if true
        | [<CompiledName("is-loading")>] IsLoading of bool
        /// Add `is-static` class if true
        | [<CompiledName("is-static")>] IsStatic of bool
        /// Add `disabled` HTMLAttr if true
        | Disabled of bool
        | Props of IHTMLProp list
        | OnClick of (MouseEvent -> unit)
        | CustomClass of string
        | Modifiers of Modifier.IModifier list

    type internal Options =
        { Level : string option
          Size : string option
          IsDisabled : bool
          Props : IHTMLProp list
          CustomClass : string option
          OnClick : (MouseEvent -> unit) option
          Modifiers : string option list }
        static member Empty =
            { Level = None
              Size = None
              IsDisabled = false
              Props = []
              CustomClass = None
              OnClick = None
              Modifiers = [] }

    let private addClass (option: Option) (result: Options) =
        { result with Modifiers = (Fable.Core.Reflection.getCaseName option |> Some)::result.Modifiers }

    let internal btnView element (options : Option list) children =
        let parseOption (result : Options) opt =
            match opt with
            | Color color -> { result with Level = ofColor color |> Some }
            // Sizes
            | Size size -> { result with Size = ofSize size |> Some }
            // Styles
            | IsLink -> { result with Level = Fable.Core.Reflection.getCaseName opt |> Some }
            | IsFullWidth
            | IsOutlined
            | IsInverted
            | IsText
            | IsRounded
            | IsExpanded -> addClass opt result
            // States
            | IsHovered state
            | IsFocused state
            | IsActive state
            | IsLoading state
            | IsStatic state -> if state then addClass opt result else result
            | Disabled isDisabled -> { result with IsDisabled = isDisabled }
            | Props props -> { result with Props = props }
            | CustomClass customClass -> { result with CustomClass = Some customClass }
            | OnClick cb -> { result with OnClick = cb |> Some }
            | Modifiers modifiers -> { result with Modifiers = modifiers |> Modifier.parseModifiers }

        let opts = options |> List.fold parseOption Options.Empty
        let classes = Helpers.classes
                        Classes.Container
                        ( opts.Level
                          ::opts.Size
                          ::opts.CustomClass
                          ::opts.Modifiers )
                        [ ]

        element
            [ yield classes
              yield Fable.Helpers.React.Props.Disabled opts.IsDisabled :> IHTMLProp
              if Option.isSome opts.OnClick then
                yield DOMAttr.OnClick opts.OnClick.Value :> IHTMLProp
              yield! opts.Props ]
            children

    /// Generate <button class="button"></button>
    let button options children = btnView button options children
    /// Generate <span class="button"></span>
    let span options children = btnView span options children
    /// Generate <a class="button"></a>
    let a options children = btnView a options children

    module Input =
        let internal btnInput typ options =
            let hasProps =
                options
                |> List.exists (fun opts ->
                    match opts with
                    | Props _ -> true
                    | _ -> false
                )

            if hasProps then
                let newOptions =
                    options
                    |> List.map (fun opts ->
                        match opts with
                        | Props props -> Props ((Type typ :> IHTMLProp) ::props)
                        | forward -> forward
                    )
                btnView (fun options _ -> input options) newOptions [ ]

            else
                btnView (fun options _ -> input options) ((Props [ Type typ ])::options) [ ]

        /// Generate <input type="reset" class="button" />
        let reset options = btnInput "reset" options
        /// Generate <input type="submit" class="button" />
        let submit options = btnInput "submit" options

    module List =

        type Option =
            | [<CompiledName("has-addons")>] HasAddons
            | [<CompiledName("is-centered")>] IsCentered
            | [<CompiledName("is-right")>] IsRight
            // | Size of ISize
            | Props of IHTMLProp list
            | CustomClass of string
            | Modifiers of Modifier.IModifier list

        type internal Options =
            //   Size : string option
            { Props : IHTMLProp list
              CustomClass : string option
              Modifiers : string option list }

            static member Empty =
                //   Size = None
                { Props = [ ]
                  CustomClass = None
                  Modifiers = [] }

        let internal ofSize size =
            match size with
            | IsSmall -> Classes.List.Size.AreSmall
            | IsMedium -> Classes.List.Size.AreMedium
            | IsLarge -> Classes.List.Size.AreLarge

    let private addListClass (option: List.Option) (result: List.Options) =
        { result with Modifiers = (Fable.Core.Reflection.getCaseName option |> Some)::result.Modifiers }

    /// Generate <div class="buttons"></div>
    let list (options : List.Option list) children =
        let parseOption (result : List.Options) opt =
            match opt with
            | List.HasAddons
            | List.IsCentered
            | List.IsRight -> addListClass opt result
            | List.Props props -> { result with Props = props }
            | List.CustomClass customClass -> { result with CustomClass = Some customClass }
            | List.Modifiers modifiers -> { result with Modifiers = modifiers |> Modifier.parseModifiers }
            // | List.Size size -> { result with Size = List.ofSize size |> Some }

        let opts = options |> List.fold parseOption List.Options.Empty
        let classes = Helpers.classes
                        Classes.List.Container
                        ( opts.CustomClass
                            // ::opts.Size
                            ::opts.Modifiers )
                        [ ]

        div (classes::opts.Props) children
