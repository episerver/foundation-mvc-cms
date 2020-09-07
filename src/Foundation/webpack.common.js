const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const node_sass_glob_importer = require('node-sass-glob-importer');

module.exports = {
    entry: {
        main : path.join(__dirname, 'Assets/js', 'index.webpack'),
    },
    resolve: {
        modules: [__dirname, "node_modules"],
    },
    output: {
        filename: "[name].webpack.min.js",
        path: path.resolve(__dirname, "Assets/js"),
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            "window.jQuery": "jquery",
            axios: "axios",
        }),
        new MiniCssExtractPlugin({
            filename: "../scss/[name].webpack.min.css",
        }),
    ],
    module: {
        rules: [
            {
                test: /\.s?css$/,
                use: [
                    {
                        loader: MiniCssExtractPlugin.loader,
                    },
                    "css-loader",
                    {
                        loader: 'sass-loader',
                        options: {
                            sassOptions: {
                                importer: node_sass_glob_importer()
                            }
                        }
                    }
                ],
            },
            {
                test: /\.(jpe?g|png|gif)$/i,
                loader: "file-loader",
                options: {
                    name: '../vendors/[name].[ext]',
                }
            },
        ],
    },
};