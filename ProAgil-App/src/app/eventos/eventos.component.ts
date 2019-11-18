import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalService } from 'ngx-bootstrap';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  titulo = 'Eventos';

  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  modoSalvar = '';

  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = true;

  registerForm: FormGroup;
  bodyDeleteEvento = '';

  imagemSelecionada = '';

  file: File;
  fileNameToUpdate: string;

  // tslint:disable-next-line: variable-name
  _filtroLista = '';
  dataAtual: string;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private serviceLocale: BsLocaleService,
    private toastr: ToastrService) {

  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  getEventos() {
    this.dataAtual = new Date().getMilliseconds().toString();
    this.eventoService.getAllEventos().subscribe(
      (evento: Evento[]) => {
        this.eventos = evento;
        this.eventosFiltrados = evento;
      },
      error => {
        this.toastr.error(`Erro ao tentar carregar eventos!`);
        console.log(error);
      }
    );
  }

  get filtroLista(): string {
    return this._filtroLista;
  }

  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation() {
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      imagemURL: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(1000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  novoEvento(template: any) {
    this.modoSalvar = 'post';
    this.openModal(template);
  }

  editarEvento(evento: Evento, template: any) {
    this.modoSalvar = 'put';
    this.openModal(template);
    this.evento = Object.assign({}, evento);
    this.fileNameToUpdate = evento.imagemURL.toString();
    this.evento.imagemURL = '';
    this.registerForm.patchValue(this.evento);
  }

  excluirEvento(evento: Evento, confirm: any) {
    this.openModal(confirm);
    this.evento = evento;
    this.bodyDeleteEvento = `Tem certeza que deseja excluir o evento:  ${evento.tema}`;
  }

  confirmarDelete(confirm: any) {
    this.eventoService.deleteEvento(this.evento).subscribe(
      () => {
        confirm.hide();

        this.getEventos();

        this.toastr.success('Deletado com sucesso');
      }, (error) => {
        console.log(error);
        this.toastr.error('Erro ao tentar deletar');
      }
    );
  }

  onFileChange(event) {
    const render = new FileReader();

    if (event.target.files && event.target.files.length) {
      this.file = event.target.files;
    }
  }

  uploadImagem(template: any) {

    if (this.modoSalvar === 'post') {
      const nomeArquivo = this.evento.imagemURL.split('\\')[2];
      this.evento.imagemURL = nomeArquivo;
      this.eventoService.postUpload(this.file, nomeArquivo)
        .subscribe(
          () => {
            this.dataAtual = new Date().getMilliseconds().toString();

            this.eventoService.postEvento(this.evento).subscribe(
              () => {
                template.hide();

                this.getEventos();
                this.toastr.success('Inserido com sucesso!');
              }, (error) => {
                console.log(error);
                this.toastr.error('Erro ao tentar inserir!');
              }
            );
          }
        );
    } else {
      this.evento.imagemURL = this.fileNameToUpdate;
      this.eventoService.postUpload(this.file, this.fileNameToUpdate)
        .subscribe(
          () => {
            this.dataAtual = new Date().getMilliseconds().toString();

            this.eventoService.putEvento(this.evento).subscribe(
              () => {
                template.hide();

                this.getEventos();

                this.toastr.success('Atualizado com sucesso!');
              }, (error) => {
                console.log(error);
                this.toastr.error('Erro ao editar!');
              }
            );
          }
        );
    }
  }

  salvarAlteracao(template: any) {
    if (this.registerForm.valid) {

      if (this.modoSalvar === 'post') {
        this.evento = Object.assign({}, this.registerForm.value);

        this.uploadImagem(template);
      } else {
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);

        this.uploadImagem(template);
      }
    }
  }
}
