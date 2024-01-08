using System;
using GarageGroup.Infra;

namespace GarageGroup.Platform.PushNotification;

public interface IPushSendHandler : IHandler<PushSendIn, Unit>;