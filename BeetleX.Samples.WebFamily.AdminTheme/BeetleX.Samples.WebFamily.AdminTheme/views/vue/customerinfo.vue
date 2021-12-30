<div>
    <auto-form v-model="data" ref="editor" size="mini" style="width:600px;margin:auto;" @command="onSave">

    </auto-form>
</div>
<script>
    export default{
        props: ['token', 'winid'],
            data(){
            return {
                data: {}
            };
        },
        methods: {
            onSave(){
                this.$confirmMsg('是否要保存数据?', () => {
                    this.$closeWindow(this.winid);
                });
            },
        },
        mounted(){

            var edit = new autoData();
            edit.addText("CompanyName", "CompanyName", true).require('The value cannot be null');
            edit.addText("ContactName", "ContactName", true).require('The value cannot be null');
            edit.addText("ContactTitle", "ContactTitle", true).require('The value cannot be null');
            edit.addText("Address", "Address", true).require('The value cannot be null');
            edit.addText("City", "City", true);
            edit.addText("Region", "Region", true);
            edit.addText("PostalCode", "PostalCode", true);
            edit.addText("Country", "Country", true);
            edit.addText("Phone", "Phone", true);
            edit.addText("Fax", "Fax", true);
            edit.addButton("save", "保存");
            if (this.token) {
                edit.setValue(this.token);
            }
            edit.bindForm(this.$refs.editor);
        }
    }
</script>