import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';

@Component({
  selector: 'app-crear-acta',
  templateUrl: './crear-acta.component.html',
  styleUrls: ['./crear-acta.component.scss']
})
export class CrearActaComponent implements OnInit {

  idSesion: number;
  objetoComiteTecnico: any = {};
  listaMiembros: Usuario[];
  nombresParticipantes: string = '';
  listaTemas: any[] = [];
  listaProposiciones: any[] = [];

  constructor ( private activatedRoute: ActivatedRoute ) { }

  ngOnInit(): void {
    this.idSesion = Number( this.activatedRoute.snapshot.params.id );
  };

};