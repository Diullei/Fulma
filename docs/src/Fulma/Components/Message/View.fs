module Components.Message.View

open Fable.Helpers.React
open Types
open Fulma
open Fulma.Elements
open Fulma.Components

let loremText =
    "Donec fermentum interdum elit, in congue justo maximus congue. Mauris tincidunt ultricies lacus, vel pulvinar diam luctus et. In vel tellus vitae dolor efficitur pulvinar eu non tortor. Nunc eget augue id nisl bibendum congue vitae vitae purus. Phasellus pharetra nunc at justo dictum rutrum. Nullam diam diam, tincidunt id interdum a, rutrum ac lorem."

let basic =
    Message.message [ ]
        [ Message.header [ ]
            [ str "Nunc finibus ligula et semper suscipit"
              Delete.delete [ ]
                [ ] ]
          Message.body [ ]
            [ str loremText ] ]

let color =
    div [ ]
        [ Message.message [ Message.Color IsInfo ]
            [ Message.header [ ]
                [ str "Nunc finibus ligula et semper suscipit"
                  Delete.delete [ ]
                    [ ] ]
              Message.body [ ]
                [ str loremText ] ]
          Message.message [ Message.Color IsDanger ]
            [ Message.header [ ]
                [ str "Nunc finibus ligula et semper suscipit"
                  Delete.delete [ ]
                    [ ] ]
              Message.body [ ]
                [ str loremText ] ] ]

let sizes =
    Message.message [ Message.Size IsSmall ]
        [ Message.header [ ]
            [ str "Nunc finibus ligula et semper suscipit"
              Delete.delete [ ]
                [ ] ]
          Message.body [ ]
            [ str loremText ] ]

let bodyOnly =
    Message.message [ ]
        [ Message.body [ ]
            [ str loremText ] ]

let root model dispatch =
    Render.docPage [ Render.contentFromMarkdown model.Intro
                     Render.docSection
                        ""
                        (Viewer.View.root basic model.BasicViewer (BasicViewerMsg >> dispatch))
                     Render.docSection
                        "### Colors"
                        (Viewer.View.root color model.ColorViewer (ColorViewerMsg >> dispatch))
                     Render.docSection
                        "### Sizes"
                        (Viewer.View.root sizes model.SizeViewer (SizeViewerMsg >> dispatch))
                     Render.docSection
                        "### Body only"
                        (Viewer.View.root bodyOnly model.BodyOnlyViewer (BodyOnlyViewerMsg >> dispatch)) ]
