using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Data;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_StatHandler(PacketSession session, IMessage packet)
	{
		C_Stat statPacket = packet as C_Stat;
		ClientSession clientSession  = session as ClientSession;

        Console.WriteLine($"C_Stat {statPacket.InitStat.Hp}, {statPacket.InitStat.Attack}, {statPacket.InitStat.AttackCool}");

        clientSession.MyPlayer = ObjectManager.Instance.Add<Player>();
        {
            clientSession.MyPlayer.Info.Name = statPacket.InitStat.Nickname;
			clientSession.MyPlayer.Info.Score = statPacket.InitStat.Score;
            clientSession.MyPlayer.Info.PosInfo.State = CreatureState.Idle;
            clientSession.MyPlayer.Info.PosInfo.MoveDir = MoveDir.Down;
            clientSession.MyPlayer.Info.PosInfo.PosX = 0;
            clientSession.MyPlayer.Info.PosInfo.PosY = 0;

			clientSession.MyPlayer.Stat.MergeFrom(statPacket.InitStat);

            clientSession.MyPlayer.Session = clientSession;
        }

        GameRoom room = RoomManager.Instance.Find(1);
        room.Push(room.EnterGame, clientSession.MyPlayer);
    }
    public static void C_MoveHandler(PacketSession session, IMessage packet)
	{
		C_Move movePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;

		//Console.WriteLine($"C_Move ({movePacket.PosInfo.PosX}, {movePacket.PosInfo.PosY})");

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.Push(room.HandleMove, player, movePacket);
	}

	public static void C_SkillHandler(PacketSession session, IMessage packet)
	{
		C_Skill skillPacket = packet as C_Skill;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.Push(room.HandleSkill, player, skillPacket);
	}
}
