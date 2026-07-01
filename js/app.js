    function mainEl(id) { return document.getElementById(id); }
    function mainNum(id) {
      const n = parseFloat(mainEl(id).value);
      return Number.isFinite(n) ? n : 0;
    }
    function mainFmt(v,d=2) {
      if(!Number.isFinite(v)) return "-";
      return Number(v.toFixed(d)).toLocaleString("zh-TW");
    }
    function mainInt(v) {
      if(!Number.isFinite(v)) return "-";
      return v.toLocaleString("zh-TW",{maximumFractionDigits:0});
    }

    const mainState = { optical:{}, selection:{}, production:{} };

    function switchMainTab(tab) {
      document.querySelectorAll(".main-tab").forEach(btn => btn.classList.toggle("active", btn.dataset.tab === tab));
      document.querySelectorAll(".main-panel").forEach(panel => panel.classList.toggle("active", panel.id === "panel-" + tab));
    }

    function toggleMainInputs() {
      const type = mainEl("lensType").value;
      const mode = mainEl("analysisSubMode").value;
      document.querySelectorAll(".main-cctv").forEach(x => x.style.display = type === "cctv" ? "grid" : "none");
      document.querySelectorAll(".main-mag").forEach(x => x.style.display = (type === "telecentric" && mode === "fromMag") ? "grid" : "none");
      document.querySelectorAll(".main-calib").forEach(x => x.style.display = mode === "fromCalib" ? "grid" : "none");
      document.querySelectorAll(".main-wd").forEach(x => x.style.display = (type === "cctv" && mode === "fromWD") ? "grid" : "none");
    }

    function calcMainOptical() {
      toggleMainInputs();

      const pixelSize = mainNum("pixelSize");
      const camW = mainNum("camW");
      const camH = mainNum("camH");
      const type = mainEl("lensType").value;
      const mode = mainEl("analysisSubMode").value;
      let mag = 0;
      let resMmPx = 0;

      if(type === "telecentric") {
        if(mode === "fromCalib") {
          resMmPx = mainNum("actualMM") / Math.max(mainNum("pixelCount"),1);
          mag = (pixelSize / 1000) / resMmPx;
        } else {
          mag = mainNum("magInput");
          resMmPx = mag > 0 ? (pixelSize / 1000) / mag : 0;
        }
      } else {
        const focal = mainNum("focalLength");
        const ext = mainNum("extLength");
        if(mode === "fromCalib") {
          const baseRes = mainNum("actualMM") / Math.max(mainNum("pixelCount"),1);
          const baseMag = (pixelSize / 1000) / baseRes;
          mag = baseMag + ext / Math.max(focal,0.0001);
        } else {
          const wd = mainNum("wdInput");
          const baseMag = focal / Math.max(wd - focal,0.0001);
          mag = baseMag + ext / Math.max(focal,0.0001);
        }
        resMmPx = mag > 0 ? (pixelSize / 1000) / mag : 0;
      }

      const fovW = resMmPx * camW;
      const fovH = resMmPx * camH;
      const resUmPx = resMmPx * 1000;
      const productW = mainNum("productW");
      const productH = mainNum("productH");
      const safe = mainNum("safeRate");
      const rateW = fovW > 0 ? productW / fovW * 100 : 0;
      const rateH = fovH > 0 ? productH / fovH * 100 : 0;

      mainState.optical = { mag, fovW, fovH, resUmPx, rateW, rateH };

      mainEl("analysisRes").textContent = mainFmt(resUmPx,4);
      mainEl("analysisMag").textContent = mainFmt(mag,4);
      mainEl("analysisFovW").textContent = mainFmt(fovW,2);
      mainEl("analysisFovH").textContent = mainFmt(fovH,2);
      mainEl("analysisRateW").textContent = mainFmt(rateW,1);
      mainEl("analysisRateH").textContent = mainFmt(rateH,1);

      const box = mainEl("productVisual");
      const ok = rateW <= safe && rateH <= safe && rateW >= 60 && rateH >= 60;
      box.className = "main-box " + (ok ? "green" : "orange");
      box.innerHTML = `
        <b>產品佔比判斷：</b>${ok ? "合理" : "需確認"}<br>
        水平佔比：${mainFmt(rateW,1)}%，垂直佔比：${mainFmt(rateH,1)}%。<br>
        建議控制在 60%～${mainFmt(safe,0)}% 之間，保留偏移與旋轉公差。
      `;
    }

    function calcMainSelection() {
      const defect = mainNum("targetDefect");
      const fov = mainNum("targetFOV");
      const safety = parseInt(mainEl("safetyFactor").value,10);
      const pixelSize = mainNum("pixelSize");
      const reqResMmPx = defect / safety;
      const reqResUmPx = reqResMmPx * 1000;
      const minPixel = Math.ceil(fov / reqResMmPx);
      const magReq = (pixelSize / 1000) / reqResMmPx;

      mainState.selection = { defect, safety, reqResUmPx, minPixel, magReq };

      mainEl("selReqRes").textContent = mainFmt(reqResUmPx,2);
      mainEl("selMinPixel").textContent = mainInt(minPixel);
      mainEl("selMagReq").textContent = mainFmt(magReq,3);
      mainEl("selectionRec").innerHTML = `
        <p>1. 相機水平像素建議大於 <b>${mainInt(minPixel)} px</b>。</p>
        <p>2. 瑕疵 ${mainFmt(defect,3)} mm 若要佔 ${safety} px，解析度需達 <b>${mainFmt(reqResUmPx,2)} μm/px</b> 或更小。</p>
        <p>3. 遠心鏡倍率可先抓接近 <b>${mainFmt(magReq,2)}X</b>。</p>
      `;
    }

    function setMainStatus(id, ok, okText, ngText) {
      const n = mainEl(id);
      n.className = "main-status " + (ok ? "main-ok" : "main-ng");
      n.textContent = ok ? okText : ngText;
    }

    function calcMainProduction() {
      const res = mainState.optical.resUmPx || 0;
      const speed = mainNum("moveSpeedMmS");
      const pixelShift = mainNum("pixelShift");
      const actualExp = mainNum("actualExposureUs");
      const maxFps = mainNum("maxFps");
      const productLen = mainNum("productLengthMm");
      const space = mainNum("spaceMm");
      const available = mainNum("availableDistanceMm");
      const batch = mainNum("batchCount");
      const batchTimeMs = mainNum("batchTimeMs");
      const batchSpeed = mainNum("batchMoveSpeedMmS");

      const speedUmUs = speed / 1000;
      const maxExp = speedUmUs > 0 ? pixelShift * res / speedUmUs : 0;
      const pitch = productLen + space;
      const fps = pitch > 0 ? speed / pitch : 0;
      const uph = fps * 3600;
      const minDefect = mainNum("minDefectPixel") * res;
      const distance = pitch * batch + batchTimeMs / 1000 * batchSpeed;

      mainState.production = { maxExp, fps, uph, pitch, minDefect, distance };

      mainEl("maxExposureUs").textContent = mainFmt(maxExp,3);
      mainEl("requiredFps").textContent = mainFmt(fps,3);
      mainEl("uph").textContent = mainInt(uph);
      mainEl("pitchMm").textContent = mainFmt(pitch,3);
      mainEl("minDefectSizeUm").textContent = mainFmt(minDefect,3);
      mainEl("lastStationDistanceMm").textContent = mainFmt(distance,3);

      setMainStatus("exposureStatus", actualExp <= maxExp, "曝光可行", "曝光過長");
      setMainStatus("fpsStatus", fps <= maxFps, "FPS 可行", "FPS 不足");
      setMainStatus("distanceStatus", distance <= available, "距離可行", "距離不足");
    }

    function renderMainSummary() {
      const o = mainState.optical, s = mainState.selection, p = mainState.production;
      const rows = [
        ["相機型號", mainEl("cameraModel").value],
        ["FOV", `${mainFmt(o.fovW,2)} × ${mainFmt(o.fovH,2)} mm`],
        ["解析度", `${mainFmt(o.resUmPx,4)} μm/px`],
        ["產品佔比", `水平 ${mainFmt(o.rateW,1)}%，垂直 ${mainFmt(o.rateH,1)}%`],
        ["瑕疵選型", `${mainFmt(s.defect,3)} mm / ${s.safety} px → ${mainFmt(s.reqResUmPx,2)} μm/px`],
        ["產能", `${mainFmt(p.fps,3)} pcs/s，${mainInt(p.uph)} UPH`],
        ["最後一站距離", `${mainFmt(p.distance,3)} mm`]
      ];
      mainEl("summaryTable").innerHTML = rows.map(([a,b]) => `<tr><th>${a}</th><td>${b}</td></tr>`).join("");
    }

    function updateMainAll() {
      calcMainOptical();
      calcMainSelection();
      calcMainProduction();
      renderMainSummary();
    }

    document.querySelectorAll(".main-tab").forEach(btn => {
      btn.addEventListener("click", () => switchMainTab(btn.dataset.tab));
    });

    document.querySelectorAll(".main-input,.main-select").forEach(x => {
      x.addEventListener("input", updateMainAll);
      x.addEventListener("change", updateMainAll);
    });

    mainEl("mainPrintBtn").addEventListener("click", () => window.print());

    updateMainAll();
