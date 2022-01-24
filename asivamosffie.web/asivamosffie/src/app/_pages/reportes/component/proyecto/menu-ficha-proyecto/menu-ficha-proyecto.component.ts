import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-menu-ficha-proyecto',
  templateUrl: './menu-ficha-proyecto.component.html',
  styleUrls: ['./menu-ficha-proyecto.component.scss']
})
export class MenuFichaProyectoComponent implements OnInit {

  @Input() indicadores: any;

  constructor() { }

  ngOnInit(): void {
    console.log(this.indicadores);
  }

}
