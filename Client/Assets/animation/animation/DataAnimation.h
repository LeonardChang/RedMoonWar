#ifndef __DATA_ANIMATION_H__
#define __DATA_ANIMATION_H__

#include "DataBase.h"

namespace Fet
{
#pragma pack(push, 1)

	struct SDataAniTiming
	{
		u32				Frame;

		SDataBaseString	SeName;

		s32				SeVolume;
		s32				SePitch;

		s32				FlashScope;
		s32				FlashColorR;
		s32				FlashColorG;
		s32				FlashColorB;
		s32				FlashColorA;
		s32				FlashDuration;

		u32				Weight;

		SDataAniTiming()
		{
		}

		~SDataAniTiming()
		{
		}
	};

	struct SDataAniFrameCell
	{
		s32				Pattern;
		s32				X;
		s32				Y;
		s32				Scaling;
		s32				Rotate;
		s32				Mirror;
		s32				Opacity;
		s32				BlendType;
	};

	struct SDataAniFrame
	{
		u32					CellNumber;
		SDataAniFrameCell*	CellList;

		SDataAniFrame()
			: CellList(0)
		{
		}
		~SDataAniFrame()
		{
			SAFE_DELETE_ARRAY(CellList);
		}
	};

	struct SDataAnimation
	{
		u32				AniID;
		SDataBaseString	Name;
		SDataBaseString	AniImg01;
		u16				AniImg01Hue;
		SDataBaseString	AniImg02;
		u16				AniImg02Hue;
		u16				Position;
		u32				FrameNumber;
		SDataAniFrame*	FrameList;
		u32				TimingNumber;
		SDataAniTiming*	TimingList;

		u32				TotalWeight;		///<	总权重（用于计算动画打点伤害等）

		SDataAnimation()
			: FrameList(0), TimingList(0)
		{
		}
		~SDataAnimation()
		{
			SAFE_DELETE_ARRAY(FrameList);
			SAFE_DELETE_ARRAY(TimingList);
		}
	};

#pragma pack(pop)

	class CDataAnimationInfos : public CDataBase
	{
	public:
		CDataAnimationInfos();
		~CDataAnimationInfos();

		bool	LoadDataFromMemory(const u8* stream, u32 length);

		const SDataAnimation*	GetAnimation(u32 uAniID) const
		{
			FetAssert(m_pAnimationList);
			FetAssert(uAniID > 0 && uAniID <= m_uAnimationNumber);

			return &m_pAnimationList[uAniID - 1];
		}

	private:
		u32					m_uAnimationNumber;
		SDataAnimation*		m_pAnimationList;
	};
}	//	Fet

#endif	//	__DATA_ANIMATION_H__
