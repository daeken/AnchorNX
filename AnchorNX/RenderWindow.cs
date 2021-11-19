using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Ryujinx.Common.Logging;
using Ryujinx.Graphics.Gpu;
using Ryujinx.Graphics.Vulkan;
using Silk.NET.Vulkan;
using static SDL2.SDL;

namespace AnchorNX {
	public class RenderWindow {
		static readonly Logger Logger = new("RenderWindow");
		static Action<string> Log = Logger.Log;

		readonly IntPtr WindowHandle;

		public RenderWindow() {
			Log("Initializing SDL");
			if(SDL_Init(SDL_INIT_EVENTS | SDL_INIT_VIDEO) != 0)
				throw new Exception();

			Log("Creating window!");
			WindowHandle = SDL_CreateWindow("AnchorNX", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 1280, 720, SDL_WindowFlags.SDL_WINDOW_VULKAN | SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
			Log(SDL_GetError());
			SDL_ShowWindow(WindowHandle);

			Log("Building renderer");
			var renderer = new VulkanGraphicsDevice(
				(instance, vk) => new SurfaceKHR((ulong) CreateWindowSurface(instance.Handle)),
				GetRequiredInstanceExtensions);
			Log(SDL_GetError());
			Log("Building GPU");
			Box.Gpu = new(renderer);
			Log(SDL_GetError());

			Log("Done with GPU creation!");
		}

		public void MainLoop() {
			Log("Starting main loop?");
			while(true) {
				if(SDL_PollEvent(out var evt) == 0) {
					
				}
				
				if(Box.Gpu.Window.ConsumeFrameAvailable())
					Box.Gpu.Window.Present(() => { });
				//Log($"Got SDL event? {evt.type}");
			}
		}

		public unsafe IntPtr CreateWindowSurface(IntPtr instance) {
			if(SDL_Vulkan_CreateSurface(WindowHandle, instance, out ulong surfaceHandle) != SDL_bool.SDL_FALSE)
				return (IntPtr) surfaceHandle;
			var errorMessage = $"SDL_Vulkan_CreateSurface failed with error \"{SDL_GetError()}\"";
			Log(errorMessage);
			throw new Exception(errorMessage);
		}

		static unsafe string GetStringFromUtf8Byte(byte* start) {
			var end = start;
			while(*end != 0) end++;

			return Encoding.UTF8.GetString(start, (int) (end - start));
		}

		// TODO: Fix this in SDL2-CS.
		[DllImport("SDL2", EntryPoint = "SDL_Vulkan_GetInstanceExtensions",
			CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_bool SDL_Vulkan_GetInstanceExtensions_Workaround(IntPtr window, out uint count, IntPtr names);

		public unsafe string[] GetRequiredInstanceExtensions() {
			if(SDL_Vulkan_GetInstanceExtensions_Workaround(WindowHandle, out var extensionsCount, IntPtr.Zero) ==
			   SDL_bool.SDL_TRUE) {
				var rawExtensions = new IntPtr[(int) extensionsCount];
				var extensions = new string[(int) extensionsCount];

				fixed(IntPtr* rawExtensionsPtr = rawExtensions) {
					if(SDL_Vulkan_GetInstanceExtensions_Workaround(WindowHandle, out extensionsCount,
						(IntPtr) rawExtensionsPtr) == SDL_bool.SDL_TRUE) {
						for(var i = 0; i < extensions.Length; i++) {
							extensions[i] = GetStringFromUtf8Byte((byte*) rawExtensions[i]);
						}

						return extensions;
					}
				}
			}

			var errorMessage = $"SDL_Vulkan_GetInstanceExtensions failed with error \"{SDL_GetError()}\"";
			Log(errorMessage);

			throw new Exception(errorMessage);
		}
	}
}