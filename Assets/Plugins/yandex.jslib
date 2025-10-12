mergeInto(LibraryManager.library, {

  ShowFullscreen: function () {
      ysdk.adv.showFullscreenAdv({
      callbacks: {
          onOpen: function(wasShown) {
            console.log('Реклама Fullscreen открылась.');
          },
          onClose: function(wasShown) {
            console.log("Реклама Fullscreen закрылась.");
          },
          onError: function(error) {
            console.log("Ошибка по рекламе Fullscreen.");
          }
      }
      })
  },

  ShowRewarded: function () {
      console.log('Запуск Rewarded рекламы.');
      ysdk.adv.showRewardedVideo({
      callbacks: {
          onOpen: () => {
            console.log('Реклама Rewarded открылась.');
          },
          onRewarded: () => {
            console.log('Реклама Rewarded просмотрена, и производим награду игроку за просмотр.');
            myGameInstance.SendMessage("StartAds", "AdsCoints");
          },
          onClose: () => {
            console.log('Реклама Rewarded закрылась.');
          }, 
          onError: (e) => {
            console.log('Ошибка по рекламе Rewarded:', e);
          }
      }
  })
  },

  InitGameplayAPI: function () {
    ysdk.features.LoadingAPI.ready();
  },

  StartGameplayAPI: function () {
    ysdk.features.GameplayAPI.start();
  },

  StopGameplayAPI: function () {
    ysdk.features.GameplayAPI.stop();
  },

  GetLanguage: function () {
    var bufferSize = lengthBytesUTF8(ysdk.environment.i18n.lang) + 1;
    var buffer = _malloc(bufferSize);

    stringToUTF8(ysdk.environment.i18n.lang, buffer, bufferSize);
    
    return buffer;
  }

});