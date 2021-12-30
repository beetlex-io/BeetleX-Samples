<div>
    <auto-form v-model="data" ref="editor" size="mini" style="width:600px;margin:auto;" @command="onSave">

    </auto-form>
</div>
<script>
    export default {
        props: ['token', 'winid'],
        data() {
            return {
                data: {}
            };
        },
        methods: {
            onSave() {
                this.$confirmMsg('是否要保存数据?', () => {
                    this.$closeWindow(this.winid);
                });
            },
        },
        mounted() {
            var edit = new autoData();
            edit.addText("LastName", "LastName", true).require('The value cannot be null');
            edit.addText("FirstName", "FirstName", true).require('The value cannot be null');
            edit.addText("Title", "Title", true).require('The value cannot be null');
            edit.addText("TitleOfCourtesy", "TitleOfCourtesy", true).require('The value cannot be null');
            edit.addDate("BirthDate", "BirthDate", true).require('The value cannot be null');;
            edit.addDate("HireDate", "HireDate", true).require('The value cannot be null');;
            edit.addText("Address", "Address", true);
            edit.addText("City", "City", true);
            edit.addText("PostalCode", "PostalCode", true);
            edit.addText("Country", "Country", true);
            var f = edit.addSelect("ReportsTo", "ReportsTo", true);
            f.dataurl = '/EmployeesSelectOptions';
            f.nulloption = true;
            edit.addButton("save", "保存");
            if (this.token) {
                edit.setValue(this.token);
            }
            edit.bindForm(this.$refs.editor);
        }
    }
</script>