import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-crear-acta',
  templateUrl: './crear-acta.component.html',
  styleUrls: ['./crear-acta.component.scss']
})
export class CrearActaComponent implements OnInit {

  idSesion: number;

  constructor ( private activatedRoute: ActivatedRoute ) { }

  ngOnInit(): void {
    this.idSesion = Number( this.activatedRoute.snapshot.params.id );
  };

};