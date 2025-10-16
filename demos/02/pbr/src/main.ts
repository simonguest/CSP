import * as BABYLON from 'babylonjs';

const app = document.querySelector<HTMLDivElement>('#app')!

// Add fullscreen styles
const style = document.createElement('style')
style.textContent = `
  * {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }

  html, body {
    width: 100%;
    height: 100%;
    overflow: hidden;
  }

  #app {
    width: 100%;
    height: 100%;
  }

  #renderCanvas {
    width: 100%;
    height: 100%;
    display: block;
    touch-action: none;
  }
`
document.head.appendChild(style)

app.innerHTML = `
  <canvas id="renderCanvas"/>
`

let canvas = document.getElementById('renderCanvas') as HTMLCanvasElement;
let engine = new BABYLON.Engine(canvas, true, { preserveDrawingBuffer: true, stencil: true });

let createScene = () => {
  let scene = new BABYLON.Scene(engine);

  let camera = new BABYLON.ArcRotateCamera('camera2', 3, 4, 2, new BABYLON.Vector3(-5, 10, 5), scene);
  camera.setTarget(BABYLON.Vector3.Zero());
  camera.attachControl(canvas, false);

  let spotLight = new BABYLON.PointLight('light2', new BABYLON.Vector3(1, 2, 1), scene);
  spotLight.intensity = 40;

  let dragonMat = new BABYLON.PBRMaterial("dragon0", scene);
  dragonMat.albedoTexture = new BABYLON.Texture("../images/dragon_tiled.png");
  dragonMat.bumpTexture = new BABYLON.Texture("../images/dragon_normal.png");
  dragonMat.metallic = 0.7;
  dragonMat.roughness = 0.1;

  var grid = {
    'h': 8,
    'w': 8
  };

  let tiledGround = BABYLON.MeshBuilder.CreateTiledGround("Tiled Ground", { xmin: -3, zmin: -3, xmax: 3, zmax: 3, subdivisions: grid });
  tiledGround.material = dragonMat;

  return scene;
};

let scene = createScene();
engine.runRenderLoop(() => {
  scene.render();
})

// A canvas/window resize event handler
window.addEventListener('resize', function () {
  engine.resize();
});