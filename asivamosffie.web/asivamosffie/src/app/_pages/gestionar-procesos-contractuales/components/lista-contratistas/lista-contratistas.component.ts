import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-lista-contratistas',
  templateUrl: './lista-contratistas.component.html',
  styleUrls: ['./lista-contratistas.component.scss']
})
export class ListaContratistasComponent implements OnInit {

  @Input() contratista: any;
  @Input() valorPagadoContratista: any;

  constructor() { }

  ngOnInit(): void {
  }

}
