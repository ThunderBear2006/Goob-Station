// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 0x6273 <0x40@keemail.me>
// SPDX-FileCopyrightText: 2024 Flesh <62557990+PolterTzi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Drugs;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Client.Drugs;

/// <summary>
///     System to handle drug related overlays.
/// </summary>
public sealed class DrugOverlaySystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    private RainbowOverlay _overlay = default!;

    public static string RainbowKey = "SeeingRainbows";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SeeingRainbowsComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SeeingRainbowsComponent, ComponentShutdown>(OnShutdown);

        SubscribeLocalEvent<SeeingRainbowsComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<SeeingRainbowsComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);

        _overlay = new();
    }

    private void OnPlayerAttached(EntityUid uid, SeeingRainbowsComponent component, LocalPlayerAttachedEvent args)
    {
        _overlayMan.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(EntityUid uid, SeeingRainbowsComponent component, LocalPlayerDetachedEvent args)
    {
        _overlay.Intoxication = 0;
        _overlay.TimeTicker = 0;
        _overlayMan.RemoveOverlay(_overlay);
    }

    private void OnInit(EntityUid uid, SeeingRainbowsComponent component, ComponentInit args)
    {
        if (_player.LocalEntity == uid)
        {
            _overlay.Phase = _random.NextFloat(MathF.Tau); // random starting phase for movement effect
            _overlayMan.AddOverlay(_overlay);
        }
    }

    private void OnShutdown(EntityUid uid, SeeingRainbowsComponent component, ComponentShutdown args)
    {
        if (_player.LocalEntity == uid)
        {
            _overlay.Intoxication = 0;
            _overlay.TimeTicker = 0;
            _overlayMan.RemoveOverlay(_overlay);
        }
    }
}