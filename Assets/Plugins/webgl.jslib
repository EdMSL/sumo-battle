mergeInto(LibraryManager.library, {
  IsMobileBrowser: function () {
    return Module.SystemInfo.mobile;
  }
});