var socialHubClient;
var userInfo;

var Comentario = function (id, texto, usuario, criadoEm, replies, curtiram) {
    var self = this;

    self.id = ko.observable(id);
    self.texto = ko.observable(texto);
    self.usuario = ko.observable(usuario);
    self.criadoEm = ko.observable(criadoEm);
    self.curtiram = ko.observableArray(curtiram);
    self.novoComentario = ko.observable('');
    self.itemToAdd = ko.observable(""),
    self.mostrarMarcaDAgua = ko.observable(true),
    self.isTypingComment = false,

    self.curtidoPorEsteUsuario = ko.computed(function () {
        var liked = false;
        $(self.curtiram()).each(function (array, user) {
            if (user.id == userInfo.Id) {
                liked = true;
            }
        });
        return liked;
    });

    self.curtir = function () {
        socialHubClient.enviarCurtirParaServidor(self.id());
    } .bind(self);

    self.descurtir = function () {
        socialHubClient.enviarDescurtirParaServidor(self.id());
    } .bind(self);

    self.adicionarNovoComentario = function (id, comentario, usuario, criadoEm) {
        self.comentarios.push(new Comentario(id, comentario, usuario, criadoEm, [], []));
    } .bind(self);

    self.sumarioDeCurtidas = ko.computed(function () {
        var summary = '';
        var sortedCurtiram = self.curtiram.sort(function (a, b) {
            var expA = (a.Id == userInfo.Id ? -1 : 1);
            var expB = (b.Id == userInfo.Id ? -1 : 1);
            return expA < expB ? -1 : 1;
        })

        $(sortedCurtiram).each(function (index, usuario) {
            if (summary.length > 0) {
                if (index == curtiram.length - 1) {
                    summary += ' e ';
                }
                else {
                    summary += ', ';
                }
            }

            if (usuario.nome == userInfo.Nome) {
                summary += 'Você';
            }
            else {
                summary += usuario.nome;
            }
        });
        if (self.curtiram().length > 1) {
            summary += ' curtiram isto';
        }
        else if (self.curtiram().length > 0) {
            summary += ' curtiu isto';
        }
        return summary;
    });

    self.comentarios = ko.observableArray([]);
    $(replies).each(function (index, reply) {
        var lk = [];
        $(reply.Curtiram).each(function (array, like) {
            lk.push({ id: like.Id, nome: like.Nome });
        });

        self.comentarios.push(
            new Comentario(reply.Id, reply.Texto, reply.Usuario, reply.CriadoEm, [], lk)
        );
    });

    self.commentKeypress = function (msg, event) {
        if (event.keyCode) {
            if (event.keyCode === 13) {
                if (self.novoComentario().trim().length > 0) {
                    socialHubClient.enviarComentarioParaServidor(self.id(), self.novoComentario());
                    self.novoComentario('');
                }
                return false;
            }
        }
        return true;
    }

    self.commentClick = function (msg, event) {
        self.mostrarMarcaDAgua(false);
    }

    self.commentFocus = function (msg, event) {
        self.isTypingComment = true;
        self.mostrarMarcaDAgua(false);
    }

    self.commentFocusout = function (msg, event) {
        if (self.novoComentario().trim().length == 0) {
            self.isTypingComment = false;
            self.mostrarMarcaDAgua(true);
        }
    }

    self.commentMouseEnter = function (msg, event) {
        self.mostrarMarcaDAgua(false);
    }

    self.commentMouseLeave = function (msg, event) {
        if (!self.isTypingComment && self.novoComentario().trim().length == 0) {
            self.mostrarMarcaDAgua(true);
        }
    }

    self.tempoDecorrido = ko.computed(function () {
        var ellapsed = '';

        var timeStampDiff = (new Date()).getTime() - parseInt(self.criadoEm().substring(6, 19));
        var s = parseInt(timeStampDiff / 1000);
        var m = parseInt(s / 60);
        var h = parseInt(m / 60);
        var d = parseInt(h / 24);

        ellapsed = getEllapsedTime(timeStampDiff);

        return ellapsed;
    });
}

var viewModel = function (model) {
    var self = this;
    self.comentarios = ko.observableArray([]);
    self.isSignalREnabled = ko.observable(model.isSignalREnabled);
    self.novoComentario = ko.observable('');
    self.mostrarMarcaDAgua = ko.observable(true),
    self.isTypingComment = false,

    $(model.Comentarios).each(function (index, comentario) {
        var curtiram = [];

        $(comentario.Curtiram).each(function (array, like) {
            curtiram.push({ id: like.Id, nome: like.Nome });
        });

        self.comentarios.push(
            new Comentario(comentario.Id, comentario.Texto, comentario.Usuario, comentario.CriadoEm, comentario.Respostas, curtiram)
        );
    });

    self.findComentarioAndAct = function (comentarioId, parent, action) {
        $(parent.comentarios()).each(function (index, comentario) {
            if (comentario.id() == comentarioId) {
                action(comentario);
                return false;
            }

            $(comentario.comentarios()).each(function (index, comentario) {
                if (comentario.id() == comentarioId) {
                    action(comentario);
                    return false;
                }
            });
        });
    }

    self.adicionarNovoComentario = function (id, comentario, usuario, criadoEm) {
        self.comentarios.unshift(new Comentario(id, comentario, usuario, criadoEm, [], []));
    } .bind(self);

    self.commentKeypress = function (msg, event) {
        if (event.keyCode) {
            if (event.keyCode === 13) {
                if (self.novoComentario().trim().length > 0) {
                    socialHubClient.enviarComentarioParaServidor(null, self.novoComentario());
                    self.novoComentario('');
                }
                return false;
            }
        }
        return true;
    }

    self.commentClick = function (msg, event) {
        self.mostrarMarcaDAgua(false);
    }

    self.commentFocus = function (msg, event) {
        self.isTypingComment = true;
        self.mostrarMarcaDAgua(false);
    }

    self.commentFocusout = function (msg, event) {
        if (self.novoComentario().trim().length == 0) {
            self.isTypingComment = false;
            self.mostrarMarcaDAgua(true);
        }
    }

    self.commentMouseEnter = function (msg, event) {
        self.mostrarMarcaDAgua(false);
    }

    self.commentMouseLeave = function (msg, event) {
        if (!self.isTypingComment && self.novoComentario().trim().length == 0) {
            self.mostrarMarcaDAgua(true);
        }
    }
};

$(function () {
    window.isSignalREnabled = false;
    requisitarComentariosDoMural();
});

function requisitarComentariosDoMural() {
    $.ajax({
        url: "/Home/PegarComentariosDoMural",
        dataType: 'json',
        success: function (data) {
            setTimeout(function () {
                data.isSignalREnabled = window.isSignalREnabled;
                window.muralViewModel = new viewModel(data);
                $('#loading-wall-messages').css('display', 'none');
                ko.applyBindings(window.muralViewModel);
                $('.wall-messages').css('display', '');

                setTimeout(function () {
                    setupHubClient();
                }, 100);
            }, 50);
        },
        error: function (jqXHR, textoStatus, errorThrown) {
            $('.ajax-error').css('display', '');
            $('#loading-wall-messages').css('display', 'none');
        }
    });
}

function setupHubClient() {
    socialHubClient = $.connection.socialHub;

    // Inicia a conexão
    $.connection.hub.start(function () {
        socialHubClient.join(userInfo.Nome);
    }).done(function () {
        window.isSignalREnabled = true;
        if (window.muralViewModel) {
            window.muralViewModel.isSignalREnabled(true);
        }
    }).fail(function () {
        alert('Conexão SignalR falhou!');
    });

    socialHubClient.atualizarCurtidas = function (comentarioId, pessoasQueCurtiram) {
        window.muralViewModel.findComentarioAndAct(comentarioId, muralViewModel, function (comentario) {
            comentario.curtiram.push({
                id: pessoasQueCurtiram.Id,
                nome: pessoasQueCurtiram.Nome
            });
        });
    };

    socialHubClient.atualizarDescurtidas = function (comentarioId, pessoasQueCurtiram) {
        window.muralViewModel.findComentarioAndAct(comentarioId, muralViewModel, function (comentario) {
            for (var i = comentario.curtiram().length - 1; i >= 0; i--) {
                if (comentario.curtiram()[i].id == pessoasQueCurtiram.Id) {
                    comentario.curtiram.splice(i, 1);
                    break;
                }
            }
        });
    };

    socialHubClient.adicionarComentario = function (parentComentarioId, newComentarioId, comment, usuario) {
        if (parentComentarioId) {
            window.muralViewModel.findComentarioAndAct(parentComentarioId, muralViewModel, function (parentComentario) {
                parentComentario.adicionarNovoComentario(
                    newComentarioId,
                    comment,
                    { Id: usuario.Id, Nome: usuario.Nome, SmallPicturePath: usuario.SmallPicturePath, MediumPicturePath: usuario.MediumPicturePath },
                    '/Date(' + (new Date()).getTime() + ')/'
                );
            });
        }
        else {
            window.muralViewModel.adicionarNovoComentario(
                newComentarioId,
                comment,
                { Id: usuario.Id, Nome: usuario.Nome, SmallPicturePath: usuario.SmallPicturePath, MediumPicturePath: usuario.MediumPicturePath },
                '/Date(' + (new Date()).getTime() + ')/'
            );
        }
    };
}

function setTimeEllapsedField(comentario) {
    comentario.timeEllapsed = ko.computed(function () {
        var ellapsed = '';

        var timeStampDiff = (new Date()).getTime() - parseInt(comentario.CriadoEm.substring(6, 19));
        var s = parseInt(timeStampDiff / 1000);
        var m = parseInt(s / 60);
        var h = parseInt(m / 60);
        var d = parseInt(h / 24);

        ellapsed = getEllapsedTime(timeStampDiff);

        return ellapsed;
    });

    $(comentario.Respostas).each(function (array, answer, index) {
        setTimeEllapsedField(answer);
    });
}

function definirCampoSumarioDeCurtidas(comentario) {
    comentario.curtiram = ko.observableArray([
        { Id: 1, Nome: "Bungle" },
        { Id: 2, Nome: "George" },
        { Id: 3, Nome: "Zippy" }
    ]);

    comentario.sumarioDeCurtidas = ko.computed(function () {
        var summary = '';
        $(comentario.curtiram).each(function (array, usuario, index) {
            if (summary.length > 0) {
                if (index == array.length - 1) {
                    summary += 'and ';
                }
                else {
                    summary += ', ';
                }
            }
            summary += usuario.Nome;
        });
        if (comentario.Curtiram.length > 0) {
            summary += ' liked this';
        }
        return summary;
    });

    $(comentario.Respostas).each(function (array, answer, index) {
        definirCampoSumarioDeCurtidas(answer);
    });
}

function curtir(comentario) {
    socialHubClient.enviarCurtirParaServidor(comentario.Id);

    findComentarioAndAct(comentario.Id, muralViewModel, function (comentario) {
        comentario.curtiram.push({
            Id: userInfo.Id,
            Nome: userInfo.Nome
        });
    });
}

function descurtir(comentario) {
    socialHubClient.enviarDescurtirParaServidor(comentario.Id);

    findComentarioAndAct(comentario.Id, muralViewModel, function (comentario) {
        for (var i = comentario.curtiram.length; i >= 0; i--) {
            if (comentario.curtiram[i].id == userInfo.Id) {
                comentario.curtiram.splice(i, 1);
            }
        }
    });
}

function getEllapsedTime(timeStampDiff) {
    var ellapsed;
    var s = parseInt(timeStampDiff / 1000);
    var m = parseInt(s / 60);
    var h = parseInt(m / 60);
    var d = parseInt(h / 24);

    if (d > 1) {
        ellapsed = 'Há ' + d + ' dias';
    }
    else if (d == 1) {
        ellapsed = 'Há ' + d + ' dia';
    }
    else if (h > 1) {
        ellapsed = 'Há ' + h + ' horas';
    }
    else if (h == 1) {
        ellapsed = 'Há ' + h + ' hora';
    }
    else if (m > 1) {
        ellapsed = 'Há ' + m + ' minutos';
    }
    else if (m == 1) {
        ellapsed = 'Há ' + m + ' minuto';
    }
    else if (s > 10) {
        ellapsed = 'Há ' + s + ' segundos';
    }
    else {
        ellapsed = 'neste momento';
    }
    return ellapsed;
}