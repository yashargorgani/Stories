$('#postCoverFile').on('change', function () {

        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {

                var img = new Image();

                img.onload = function () {
                    var canvas = document.createElement("canvas");
                    var ctx = canvas.getContext("2d");
                    ctx.drawImage(img, 0, 0);

                    var MAX_WIDTH = 1200;
                    var MAX_HEIGHT = 1200;
                    var width = img.width;
                    var height = img.height;

                    if (width > height) {
                        if (width > MAX_WIDTH) {
                            height *= MAX_WIDTH / width;
                            width = MAX_WIDTH;
                        }
                    } else {
                        if (height > MAX_HEIGHT) {
                            width *= MAX_HEIGHT / height;
                            height = MAX_HEIGHT;
                        }
                    }
                    canvas.width = width;
                    canvas.height = height;
                    var ctx = canvas.getContext("2d");
                    ctx.drawImage(img, 0, 0, width, height);

                    dataurl = canvas.toDataURL('image/png');
                    document.getElementById('output').src = dataurl;
                    document.getElementById('StoryCoverImgResized').value = dataurl;
                }

                img.src = e.target.result;
            }

            reader.readAsDataURL(this.files[0]);
        }
    });