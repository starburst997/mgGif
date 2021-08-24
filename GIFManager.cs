﻿using System.Collections.Concurrent;

namespace jd.boivin.unity.gif
{
    // Allow to recycle GIF instance
    public static class GIFManager
    {
        private static readonly ConcurrentQueue<GIFDecoder> Decoders = new ConcurrentQueue<GIFDecoder>();

        public static void Initialize()
        {
            for (var i = 0; i < 4; i++)
                Decoders.Enqueue(new GIFDecoder());
        }
        
        public static void Clear()
        {
            while (Decoders.TryDequeue(out var decoder))
            {
                decoder.Dispose();
            }
        }
        
        public static void Return(GIFDecoder decoder)
        {
            decoder.Clear();
            
            Decoders.Enqueue(decoder);
        }

        public static GIFDecoder Get()
        {   
            if (Decoders.TryDequeue(out var decoder))
            {
                decoder.Reset();
                return decoder;
            }
            
            var newDecoder = new GIFDecoder();
            newDecoder.Reset();

            return newDecoder;
        }
    }
}