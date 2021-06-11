<div>
    <el-form :inline="true" size="mini" :model="record" label-width="120px" ref="dataform">
        <el-form-item label="用户名" prop="name" :rules="name_rules">
            <el-input size="mini" v-model="record.name"></el-input>
        </el-form-item>
        <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="submitForm">确定</el-button>
    </el-form>
    <p>{{hello.result}}</p>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                name_rules: [{ required: true, message: '值不能为空！', trigger: 'blur' },],
                record: {
                    name: null,
                },
                hello: new beetlexAction('/hello', null, '')
            };
        },
        methods: {
            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.hello.asyncget(this.record).then(r => {
                            this.resetForm();
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
