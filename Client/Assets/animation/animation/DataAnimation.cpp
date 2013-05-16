#include "DataAnimation.h"

using namespace Fet;

CDataAnimationInfos::CDataAnimationInfos()
	: m_uAnimationNumber(0)
	, m_pAnimationList(0)
{
}

CDataAnimationInfos::~CDataAnimationInfos()
{
	SAFE_DELETE_ARRAY(m_pAnimationList);
}

bool CDataAnimationInfos::LoadDataFromMemory(const u8* stream, u32 length)
{
	FetAssert(!m_bInited);

	FetAuxReader reader(stream, length);

	m_uAnimationNumber		= reader.PopRef<u32>();

	m_pAnimationList		= new SDataAnimation[m_uAnimationNumber];
	if (!m_pAnimationList)
		return false;

	for (u32 idx = 0; idx < m_uAnimationNumber; ++idx)
	{
		SDataAnimation* pAni = &m_pAnimationList[idx];

		//	id
		pAni->AniID			= reader.PopRef<u32>();
		FetAssert(pAni->AniID == (idx + 1));

		//	name
		if (!pAni->Name.Alloc(reader.CurrPtr<wchar_t>(), reader.PopRef<u32>()))
		{
			SAFE_DELETE_ARRAY(m_pAnimationList);
			return false;
		}
		reader.PopPtr<wchar_t>(pAni->Name.Length);

		//	ani image 01
		if (!pAni->AniImg01.Alloc(reader.CurrPtr<wchar_t>(), reader.PopRef<u32>()))
		{
			SAFE_DELETE_ARRAY(m_pAnimationList);
			return false;
		}
		reader.PopPtr<wchar_t>(pAni->AniImg01.Length);

		//	ani image 01 hue
		pAni->AniImg01Hue		= reader.PopRef<u16>();

		//	ani image 02
		if (!pAni->AniImg02.Alloc(reader.CurrPtr<wchar_t>(), reader.PopRef<u32>()))
		{
			SAFE_DELETE_ARRAY(m_pAnimationList);
			return false;
		}
		reader.PopPtr<wchar_t>(pAni->AniImg02.Length);

		//	ani image 02 hue
		pAni->AniImg02Hue		= reader.PopRef<u16>();

		//	position
		pAni->Position			= reader.PopRef<u16>();

		//	frames
		pAni->FrameNumber		= reader.PopRef<u32>();
		pAni->FrameList			= new SDataAniFrame[pAni->FrameNumber];
		if (!pAni->FrameList)
		{
			SAFE_DELETE_ARRAY(m_pAnimationList);
			return false;
		}

		for (u32 frmidx = 0; frmidx < pAni->FrameNumber; ++frmidx)
		{
			SDataAniFrame* pFrm = &pAni->FrameList[frmidx];

			pFrm->CellNumber = reader.PopRef<u32>();
			pFrm->CellList	= new SDataAniFrameCell[pFrm->CellNumber];
			if (!pFrm->CellList)
			{
				SAFE_DELETE_ARRAY(m_pAnimationList);
				return false;
			}

			for (u32 cellidx = 0; cellidx < pFrm->CellNumber; ++cellidx)
			{
				SDataAniFrameCell* pCell = &pFrm->CellList[cellidx];

				pCell->Pattern		= reader.PopRef<s32>();
				pCell->X			= reader.PopRef<s32>();
				pCell->Y			= reader.PopRef<s32>();
				pCell->Scaling		= reader.PopRef<s32>();
				pCell->Rotate		= reader.PopRef<s32>();
				pCell->Mirror		= reader.PopRef<s32>();
				pCell->Opacity		= reader.PopRef<s32>();
				pCell->BlendType	= reader.PopRef<s32>();
			}
		}

		//	timing
		pAni->TimingNumber		= reader.PopRef<u32>();
		pAni->TimingList		= new SDataAniTiming[pAni->TimingNumber];
		if (!pAni->TimingList)
		{
			SAFE_DELETE_ARRAY(m_pAnimationList);
			return false;
		}

		pAni->TotalWeight		= 0;

		for (u32 timingidx = 0; timingidx < pAni->TimingNumber; ++timingidx)
		{
			SDataAniTiming* pTiming = &pAni->TimingList[timingidx];

			pTiming->Frame			= reader.PopRef<u32>();

			if (!pTiming->SeName.Alloc(reader.CurrPtr<wchar_t>(), reader.PopRef<u32>()))
			{
				SAFE_DELETE_ARRAY(m_pAnimationList);
				return false;
			}
			reader.PopPtr<wchar_t>(pTiming->SeName.Length);

			pTiming->SeVolume		= reader.PopRef<s32>();
			pTiming->SePitch		= reader.PopRef<s32>();

			pTiming->FlashScope		= reader.PopRef<s32>();
			FetAssert(pTiming->FlashScope >= 0 && pTiming->FlashScope <= 3);

			pTiming->FlashColorR	= reader.PopRef<s32>();
			FetAssert(pTiming->FlashColorR >= 0 && pTiming->FlashColorR <= 255);

			pTiming->FlashColorG	= reader.PopRef<s32>();
			FetAssert(pTiming->FlashColorG >= 0 && pTiming->FlashColorG <= 255);

			pTiming->FlashColorB	= reader.PopRef<s32>();
			FetAssert(pTiming->FlashColorB >= 0 && pTiming->FlashColorB <= 255);

			pTiming->FlashColorA	= reader.PopRef<s32>();
			FetAssert(pTiming->FlashColorA >= 0 && pTiming->FlashColorA <= 255);

			pTiming->FlashDuration	= reader.PopRef<s32>();
			FetAssert(pTiming->FlashDuration >= 1 && pTiming->FlashDuration <= 200);

			pTiming->Weight			= (u32)(pTiming->FlashColorA * pTiming->FlashDuration);
			if (pTiming->Weight > 0)
			{
				pAni->TotalWeight += pTiming->Weight;
			}
		}
	}

	m_bInited = true;

	return true;
}