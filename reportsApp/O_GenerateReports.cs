using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace reportsApp
{
    public static class O_GenerateReports
    {
        //[FunctionName("O_GenerateReports")]
        //public static async Task<VideoFileInfo[]> TranscodeVideo(
        //   [OrchestrationTrigger] DurableOrchestrationContext ctx,
        //   TraceWriter log)
        //{
        //    var videoLocation = ctx.GetInput<string>();
        //    var bitRates = await ctx.CallActivityAsync<int[]>("A_GetTranscodeBitrates", null);
        //    var transcodeTasks = new List<Task<VideoFileInfo>>();

        //    foreach (var bitRate in bitRates)
        //    {
        //        var info = new VideoFileInfo() { Location = videoLocation, BitRate = bitRate };
        //        var task = ctx.CallActivityAsync<VideoFileInfo>("A_GenerateReport", info);
        //        transcodeTasks.Add(task);
        //    }

        //    var transcodeResults = await Task.WhenAll(transcodeTasks);
        //    return transcodeResults;
        //}
    }
}
