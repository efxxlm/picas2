import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-lista-contratistas-rlc',
  templateUrl: './lista-contratistas-rlc.component.html',
  styleUrls: ['./lista-contratistas-rlc.component.scss']
})
export class ListaContratistasRlcComponent implements OnInit {

  @Input() contratista: any;
  @Input() valorPagadoContratista: any;

  constructor() { }

  ngOnInit(): void {
  }

}
