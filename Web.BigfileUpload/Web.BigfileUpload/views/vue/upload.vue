<div>
    <div class="row">
        <div class="form-group">
            <label for="exampleInputEmail1"></label>
            <input type="file" class="form-control" placeholder="name" multiple @change="onSelectFile">
        </div>
    </div>
    <div class="row">
        <div v-for="item in items" class="upload-item">
            <div>{{item.name}}[{{item.loadSize}}/{{item.size}}]</div>
            <el-progress :stroke-width="2" :percentage="item.percent" type="line" :show-text="false"></el-progress>
        </div>

    </div>
</div>
<script>
    {
        data() {
            return { items: [] }
        },
        methods: {
            onSelectFile(e) {
                var files = e.target.files;
                if (files.length > 0) {
                    for (i = 0; i < files.length; i++) {
                        var item = new FileUploader(files[i], 1024 * 128);
                        this.items.push(item);
                        item.upload("Upload");
                    }
                }
            },
        }
    }
</script>