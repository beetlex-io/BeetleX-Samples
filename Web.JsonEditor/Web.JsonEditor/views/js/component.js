/*
* Generate js with vuejs Copyright © ikende.com 2019 email:henryfan@msn.com 
*/
var __header="";
 __header += '    <div class="navbar navbar-inverse navbar-fixed-top">';
 __header += '        <div class="container">';
 __header += '';
 __header += '            <span class="navbar-brand " href="http://beetlex.io" target="_blank">Json Editor</span>';
 __header += '                ';
 __header += '                <ul class="nav navbar-nav navbar-right ">';
 __header += '                    <li><a href="http://beetlex.io" target="_blank">About</a></li>';
 __header += '                </ul>';
 __header += '          ';
 __header += '        </div>';
 __header += '    </div>';
var __footer="";
 __footer += '    <div class="navbar navbar-inverse navbar-fixed-bottom" style="height:26px;">';
 __footer += '        <div class="container">';
 __footer += '            <p style="text-align:center;">copyright © 2019-2020 <a href="http://beetlex.io" target="_blank">beetlex.io</a>  email:henryfan@msn.com | wechat:henryfan128</p>';
 __footer += '        </div>';
 __footer += '    </div>';


    Vue.component('page-header', {
        props: ['info', 'page'],
        data: function () {
            return {
                count: 0
            }
        },
        template: __header
    });

    Vue.component('page-footer', {
        props: ['info'],
        data: function () {
            return {
                count: 0
            }
        },
        template: __footer,
    })

