import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-lista-contratistas-gtlc',
  templateUrl: './lista-contratistas-gtlc.component.html',
  styleUrls: ['./lista-contratistas-gtlc.component.scss']
})
export class ListaContratistasGtlcComponent implements OnInit {

  @Input() contratista: any;
  @Input() valorPagadoContratista: any;

  constructor() { }

  ngOnInit(): void {
  }

}
