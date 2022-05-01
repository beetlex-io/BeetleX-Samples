<div>
    <el-form size="mini" :model="record" label-width="120px" ref="dataform">
        <el-row>
            <el-col :span="24">
                <el-form-item label="标题" prop="Title" :rules="Title_rules"><el-input size="mini" v-model="record.Title"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="分类" prop="Category" :rules="Category_rules"><el-input size="mini" v-model="record.Category"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="标签(空格分割)" prop="Tag"><el-input size="mini" v-model="record.Tag"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="概述" prop="Content" :rules="Summary_rules"><el-input size="mini" :rows="6" v-model="record.Summary" type="textarea"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="内容" prop="Content" :rules="Content_rules"><el-input size="mini" :rows="20" v-model="record.Content" type="textarea"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col span="24" style="text-align:center">
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="submitForm">添加</el-button>
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="onClean">清空</el-button>
            </el-col>
        </el-row>
    </el-form>
</div>
<script>
    export default {
        props: ['post'],
        data() {
            return {
                Title_rules: [{ required: true, message: '值不能为空！', trigger: 'blur' },],
                Category_rules: [{ required: true, message: '值不能为空！', trigger: 'blur' },],
                Content_rules: [{ required: true, message: '值不能为空！', trigger: 'blur' },],
                Summary_rules: [{ required: true, message: '值不能为空！', trigger: 'blur' },],
                record: {
                    ID: null,
                    Title: null,
                    Category: null,
                    Tag: null,
                    Content: null,
                    Summary: null,
                }
            };
        },
        watch: {
            post(val) {
                console.log('set post', val);

                if (val) {

                    this.$get('/GetDocument', { id: val.ID }).then(r => {
                        this.record = r;
                    });
                }
            }
        },
        methods: {

            onClean() {
                this.record = {
                    ID: null,
                    Title: null,
                    Category: null,
                    Tag: null,
                    Content: null,
                    Summary: null,
                };
            },
            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.$post('/AddDocument', this.record).then(r => {
                            this.$successMsg('文档已保存!');
                            this.onClean();
                        });
                    }
                });
            },
            resetForm() {
                this.$refs['dataform'].resetFields();
            }
        },
        mounted() {
        }
    }
</script>
