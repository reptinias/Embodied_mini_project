using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthNormalFeature : ScriptableRendererFeature
{
    class Pass : ScriptableRenderPass
    {
        private Material material;
        private List<ShaderTagId> shaderTags;
        private FilteringSettings filteringSettings;
        private RenderTargetHandle renderTargetHandle;
        public Pass(Material material)
        {
            this.material = material;

            this.shaderTags = new List<ShaderTagId>()
            {
                new ShaderTagId("DepthOnly")
            };

            this.filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            renderTargetHandle.Init("_DepthNormalsTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(renderTargetHandle.id, cameraTextureDescriptor,FilterMode.Point);
            ConfigureTarget(renderTargetHandle.Identifier());
            ConfigureClear(ClearFlag.All, Color.black);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var drawSettings = CreateDrawingSettings(shaderTags, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            drawSettings.overrideMaterial = material;
            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(renderTargetHandle.id);
        }

    }

    private Pass pass;

    /// <inheritdoc/>
    public override void Create()
    {
        Material material = CoreUtils.CreateEngineMaterial("Hidden/internal-DepthNormalsTexture");
        this.pass = new Pass(material);

        // Configures where the render pass should be injected.
        pass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}


