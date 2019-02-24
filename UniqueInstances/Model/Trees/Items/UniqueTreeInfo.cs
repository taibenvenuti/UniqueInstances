using System;

namespace UniqueInstances
{
    [Serializable]
    public class UniqueTreeInfo
    {
        public float m_minScale;
        public float m_maxScale;
        public float m_minBrightness;
        public float m_maxBrightness;
        public TreeInfo.Variation[] m_variations;

        public UniqueTreeInfo(TreeInfo oldInfo)
        {
            m_minScale = oldInfo.m_minScale;
            m_maxScale = oldInfo.m_maxScale;
            m_minBrightness = oldInfo.m_minBrightness;
            m_maxBrightness = oldInfo.m_maxBrightness;
            m_variations = oldInfo.m_variations;
        }

        public void Load(TreeInfo newInfo)
        {
            newInfo.m_minScale = m_minScale;
            newInfo.m_maxScale = m_maxScale;
            newInfo.m_minBrightness = m_minBrightness;
            newInfo.m_maxBrightness = m_maxBrightness;
            newInfo.m_variations = m_variations;
        }
    }
}
