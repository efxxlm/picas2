import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-contratistas-gbftrec',
  templateUrl: './contratistas-gbftrec.component.html',
  styleUrls: ['./contratistas-gbftrec.component.scss']
})
export class ContratistasGbftrecComponent implements OnInit {

  @Input() contratista: any;
  @Input() valorPagadoContratista: any;

  constructor() { }

  ngOnInit(): void {
  }

}
